# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Unity **6000.2.1f1** (Unity 6) 2D narrative game. The Unity project lives in `Cod-Main/` — open that folder in the Unity Hub, not the repo root. Source code, design notes, and dialog are predominantly **German**; match the existing language when editing comments, dialog assets, and UI copy.

## Custom Git Packages

Several gameplay systems are consumed as Unity Package Manager git dependencies (see `Cod-Main/Packages/manifest.json`). Their source lives outside this repo; read them under `Cod-Main/Library/PackageCache/<pkg>@<hash>/Runtime` when you need implementation details:

- `com.cod.playerinput` — `PlayerController`, `CameraMovement`, `InputDispatcher`. Types live in the `Runtime.Scripts.PlayerInput` namespace (e.g. `MoveDirection`). `PlayerController` exposes `OnMovementStarted` / `OnMovementEnded` events that gameplay scripts subscribe to (see `AnimationTrigger.cs`).
- `com.cod.dialog-builder` — `DialogTreeRunner`, `DialogTree`, `Blackboard`, `SubtitlePresenter`, interfaces `IDialogStarter` / `IDialogReceiver`. Dialog tree assets are authored under `Cod-Main/Assets/Dialoge/`.
- `com.cod.interactionbuilder` — `InteractionHandler`, `InteractionData`, `SequenceRunner`, `ScriptedSequence`, `Raycaster`. The `Raycaster` exposes `isDialogRunning`, which other systems (MainMenu, JournalMenu) toggle to gate world interaction while UI is open.
- `com.cod.audioplayer` — audio trigger/player utilities used by `AudioClipPlayer.cs` etc.
- `com.cod.csvmanager` — CSV-backed data (likely used by dialog/localization).

When the package source is updated upstream, bump the manifest hash by deleting the package line and re-adding it via Package Manager (or `rm -rf Cod-Main/Library/PackageCache/com.cod.*` and let Unity refetch).

## Audio: Wwise, not Unity AudioSource

Audio is driven by **Wwise** (the `AK.Wwise.*` csproj files and `Cod-Main/Cod-Main_WwiseProject/` are the integration). Do not add `AudioSource` components for SFX/music; call into Wwise. Volume is controlled through RTPCs set from `Assets/Scripts/Audio/InGameAudioSettings.cs` (a `ScriptableObject`): `VOL_Master`, `VOL_Music`, `VOL_SFX`. The Wwise project (`.wproj`) must be opened in the Wwise authoring tool to add/modify events, buses, or RTPCs — soundbanks are generated under `Cod-Main/Cod-Main_WwiseProject/GeneratedSoundBanks/`.

`MusicPlayer.cs` uses a raw `AudioSource` and is the legacy exception — prefer Wwise for new audio.

## Runtime Bootstrap & Global Scene Flow

- `Assets/Scripts/Setup/Bootstrapper.cs` runs at `RuntimeInitializeLoadType.BeforeSceneLoad` and instantiates `Resources/Prefabs/Global` as `DontDestroyOnLoad`. Anything that must exist across all scenes (managers, global UI, audio settings) goes on that prefab — don't rely on per-scene singletons.
- `GameManager` fires a static `GameStarted` event on `Awake`; gameplay systems subscribe to this instead of using `Start` ordering.
- `RoomManager` implements a room-swap pattern for the house: one outdoor object plus a list of indoor rooms. `ActivateRoom()` toggles `GameObject.SetActive` and emits `OnRoomChanged(bool isOutside)`. `RoomTrigger` components on colliders drive the transitions. `kitchenOnlyObject` is a blocker object only shown when the kitchen is active — preserve that behavior when refactoring.

## UI

Menus use **Unity UI Toolkit** (`UIDocument` + `UIElements`), not uGUI Canvas. See `Assets/Scripts/UI/MainMenu.cs` and `JournalMenu.cs` for the pattern: `Q<Button>("Name")` lookups against `rootVisualElement`, and both toggle `Raycaster.isDialogRunning` to suppress world interaction while a menu is visible. USS lives at the Assets root (`MainMenuStyle.uss`, `MenuStyle.uss`, `hideLabel.uss`).

## Namespaces

The in-repo scripts use a mix of `DefaultNamespace` (legacy), `Audio`, `Setup`, `UI`, and `Runtime.Scripts.Interactables`. Package types come from `Runtime.Scripts.PlayerInput` / `Runtime.Scripts.Interactables`. There is no unified root namespace — match the folder's existing convention when adding files.

## Build / Run / Test

There is no CLI build pipeline. Open the project in Unity Hub (must be 6000.2.1f1) and use the Editor:
- **Play:** Unity Editor Play button. `Bootstrapper` runs regardless of entry scene.
- **Build:** File → Build Profiles (Unity 6 terminology).
- **Tests:** Window → General → Test Runner (`com.unity.test-framework` 1.5.1 is installed, but no test assemblies exist yet in `Assets/`).
- **Regenerate `.csproj` / `.sln`:** Edit → Preferences → External Tools → *Regenerate project files*. The `.csproj`/`.sln` files are gitignored and recreated by Unity.

## Conventions & Gotchas

- The repo root contains a duplicate `gitattributes` (no leading dot) alongside `.gitattributes` — don't "fix" this by deleting one without checking; the non-dotted file appears to be intentional (possibly historical). Confirm with the team before touching.
- `Library/`, `Temp/`, `Logs/`, `UserSettings/`, and all `*.csproj` / `*.sln` are gitignored. Never commit them.
- Scenes are numbered (`Scene 1.unity` … `Scene 4.unity`) plus working copies (e.g. `Scene 2 - vivi 260207.unity`, `Scene 2 copy for animation.unity`). The dated/named variants are WIP branches of the canonical numbered scene — confirm which one is live before editing.
- `WwiseUnityIntegration_*.zip` in `Cod-Main/` are the vendor's source archives for the Wwise Unity integration; leave them alone.
