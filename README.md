# Project Overview


## 1. UI Panel System
### `UIPanelBase.cs`
`BasePanel` provides a reusable, fade-in/fade-out implementation for any popup or window. It handles:
- Toggling a `CanvasGroup` (alpha, interactable, raycasts)
- Optionally driving an `Animator` (with “Open”/“Close” triggers)
- Wiring “Open” and “Close” buttons via `ActionContainer` or `Button.onClick`
- Preventing multiple concurrent show/hide calls

#### How It Works

1. **Awake Setup**  
   - Caches (or finds) a `CanvasGroup`.
   - Registers each “open” button or action container to call `Show()`.
   - Registers each “close” button or action container to call `Hide()`.
   - Starts with `canvasGroup.alpha = 0` and `gameObject.SetActive(false)`.

2. **Show()**  
   - If not already visible or animating:
     - Sets `isVisible = true`, `isAnimating = true`, and `gameObject.SetActive(true)`
     - If an `Animator` with an “Open” parameter exists, triggers it; otherwise, does a DOTween fade from alpha 0 → 1
     - Disables `interactable/blocksRaycasts` until animation completes
     - When fade/animation finishes, calls `OnShowAnimationComplete()`, which re-enables interaction and fires the `Shown` event

3. **Hide()**  
   - If currently visible and not animating:
     - Sets `isVisible = false`, `isAnimating = true`
     - If an `Animator` with a “Close” parameter exists, triggers it; otherwise, does a DOTween fade from alpha 1 → 0
     - After fade/animation, calls `OnHideAnimationComplete()`, which disables interaction, deactivates the GameObject, and fires the `Hidden` event

4. **Events**  
   - `public event Action Shown;` — invoked when the panel is fully visible  
   - `public event Action Hidden;` — invoked when the panel has finished hiding

5. **Animator Parameter Check**  
   - `HasAnimatorParameter(stringHash)` loops through `animator.parameters` to detect “Open”/“Close” triggers. If missing, fallback to fade.

#### Usage

1. **Create a new panel GameObject** in your UI Canvas.
2. **Add a `CanvasGroup`** and `Animator` (optional) components.
3. **Attach `BasePanel` (or a subclass)**.  
   ```csharp
   public class SettingsPanel : BasePanel
   {
       // Optionally override OnShown/OnHidden or add extra behavior
   }


## 2.CarDriftController.cs
High-performance car physics with built-in drifting, using only Unity’s Rigidbody. 
Supports:
    Forward/brake forces from Vertical input
    Speed-scaled steering from Horizontal input
    Two traction modes (normal vs. drift when holding Space)
    Max‐speed clamping
    Rigidbody constraints to keep the car upright

#### How It Works
Awake: Configures Rigidbody (mass, drag, angularDrag) and freezes X/Z rotation.

Update: Reads _inputVertical, _inputHorizontal, and _isDrifting (Space).

FixedUpdate:
    Applies forward/brake force along transform.forward.
    Rotates around Y based on horizontal input scaled by current speed.
    Splits velocity into forward and lateral components; scales lateral by NormalTraction or DriftTraction.
    Clamps velocity to MaxSpeed if exceeded.

#### Usage
Attach CarDriftController to your car GameObject (must have a Rigidbody).
    
**Configure in Inspector**
Physics: Mass, AccelerationForce, BrakeForce, MaxSpeed, TurnSpeed
Drift: NormalTraction, DriftTraction, DriftKey (default Space)





## 3.SkillSystem.cs
Manages multiple skills (SkillData: type, level, current XP) and handles XP gain and level-up using ActionContainerInt events.
AddXp(skillIndex, amount): Adds XP, fires _onXpChanged, auto-levels when XP ≥ threshold, then fires _onSkillLeveled.
Upgrade via Event: Subscribes to _upgradeSkillRequested; if XP ≥ needed, consumes XP, increments level, fires _onSkillLeveled.

### Usage
Attach SkillSystem to a GameObject.
Populate _skills list in Inspector.

Wire ActionContainerInt fields:
_upgradeSkillRequested for level-up requests.
_onXpChanged for UI updates.
_onSkillLeveled for effects/sounds.

**Call AddXp(index, amount)** when the player earns XP; fire _upgradeSkillRequested.FireAction(index) to attempt manual upgrade.




## 4. Action Container System (DI)
ActionContainerBool.cs, ActionContainerInt.cs, ActionContainerFloat.cs, ActionContainerTransform.cs
Action Containers provide typed, inspector-assignable event channels for dependency injection. They allow any component to subscribe or fire events without direct references, promoting loose coupling.

Each container exposes a public Action<T> action (e.g., Action<int> in ActionContainerInt) that other scripts can subscribe to in Awake/OnEnable.

Call FireAction(value) on the container to invoke all subscribers.

Assign containers in the Inspector to link producers and consumers without hardcoding dependencies.

### How It Works
Declare an ActionContainerX field and assign it in Inspector (e.g., public ActionContainerInt _onXpChanged;).

Subscribe to container.action += YourHandler in Awake/OnEnable of any script.

Fire events by calling container.FireAction(payload) (e.g., container.FireAction(1) for skill index 1).

Unsubscribe in OnDestroy/OnDisable (container.action -= YourHandler) to avoid memory leaks.

### Usage
Create an ActionContainerX asset (via Create → ScriptableObjects → ActionContainerInt, etc.).

In any producer script (e.g., SkillSystem), drag the container into _onSkillLeveledAction and call FireAction(skillIndex) when leveling.

**In any consumer script (e.g., EffectsSystem), subscribe in Awake:**

    _onSkillLeveledAction.action += HandleSkillLeveled;

Then implement HandleSkillLeveled(int skillIndex) to play effects, update UI, etc.

**Unsubscribe in OnDestroy:**

    _onSkillLeveledAction.action -= HandleSkillLeveled;
This setup allows you to wire events purely in the Inspector—no hardcoded references—enabling clean, modular communication between subsystems.


etc...
