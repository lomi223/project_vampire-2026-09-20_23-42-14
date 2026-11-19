using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class CharacterMovement : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sequence activeSequence;

    // Reusable stage positions (adjust to match your camera view)
    private static readonly Dictionary<string, Vector3> StageAnchors = new Dictionary<string, Vector3>
    {
        { "offscreen_left",  new Vector3(-12f, 0f, 0f) },
        { "left",            new Vector3(-4f,  0f, 0f) },
        { "middle",          new Vector3(0f,   0f, 0f) },
        { "right",           new Vector3(4f,   0f, 0f) },
        { "offscreen_right", new Vector3(12f,  0f, 0f) }
    };

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Call this from your Dialogue CSV runner
    public void PlayMovement(string movementName)
    {
        // If the player clicks fast to skip dialogue, snap the previous movement
        // to its final state instead of leaving the character stuck mid-air
        if (activeSequence.isAlive)
        {
            activeSequence.Complete();
        }

        switch (movementName.Trim().ToLower())
        {
            case "enter_left":
                activeSequence = EnterLeft();
                break;

            case "slide_to_middle":
                activeSequence = SlideToMiddleAndThink();
                break;

            case "hop":
                activeSequence = QuickHop();
                break;

            case "fade_out":
                activeSequence = FadeOut();
                break;

            default:
                Debug.LogWarning($"Movement '{movementName}' is not coded in CharacterMovement.");
                break;
        }
    }

    // --- Coded Movement Presets ---

    private Sequence EnterLeft()
    {
        // Instantly place offscreen and invisible before starting
        transform.position = StageAnchors["offscreen_left"];
        SetAlpha(0f);

        return Sequence.Create()
            .ChainCallback(() => ChangePicture("default"))
            // Move in AND fade in at the exact same time using Group()
            .Chain(Tween.Position(transform, endValue: StageAnchors["left"], duration: 0.6f, ease: Ease.OutCubic))
            .Group(Tween.Alpha(spriteRenderer, endValue: 1f, duration: 0.4f));
    }

    private Sequence SlideToMiddleAndThink()
    {
        return Sequence.Create()
            // Step 1: Slide to middle
            .Chain(Tween.Position(transform, endValue: StageAnchors["middle"], duration: 0.7f, ease: Ease.InOutQuad))
            // Step 2: Pause briefly, then swap picture state
            .ChainDelay(0.15f)
            .ChainCallback(() => ChangePicture("thinking"));
    }

    private Sequence QuickHop()
    {
        float startY = transform.position.y;

        return Sequence.Create()
            .Chain(Tween.PositionY(transform, endValue: startY + 0.6f, duration: 0.15f, ease: Ease.OutQuad))
            .Chain(Tween.PositionY(transform, endValue: startY, duration: 0.15f, ease: Ease.InQuad));
    }

    private Sequence FadeOut()
    {
        return Sequence.Create()
            .Chain(Tween.Alpha(spriteRenderer, endValue: 0f, duration: 0.5f, ease: Ease.Linear));
    }

    // --- Helpers ---

    private void SetAlpha(float a)
    {
        Color c = spriteRenderer.color;
        c.a = a;
        spriteRenderer.color = c;
    }

    private void ChangePicture(string stateName)
    {
        // Hook this into your CharacterInfo / CustomSpriteAnimator state lookup
        Debug.Log($"Swapping sprite to state: {stateName}");
    }
}