using Godot;
using System;
using TopDownGame.Skill;

namespace TopDownGame.UI;

internal partial class SkillSlotControl : Control
{
  [Export]
  private Button _button;

  [Export]
  private TextureRect _iconRect;

  [Export]
  private TextureProgressBar _cooldownBar;

  [Export]
  private Label _cooldownLabel;

  [Export]
  private Label _keybind;

  [Export]
  private SkillResource _skill;

  [Export]
  private string inputActionName = "ActionBar1Pressed";

  private float _remaining = 0f;

  public override void _Ready()
  {
    if (_skill?.Skill is ISkill skill)
    {
      InitializeSkillSlot();
    }
  }

  public override void _Process(double delta)
  {
    // Handle keybind input - only trigger once per press
    if (Input.IsActionJustPressed(inputActionName))
    {
      OnButtonPressed();
    }

    if (_remaining > 0f)
    {
      _remaining -= (float)delta;
      if (_remaining <= 0f)
      {
        _remaining = 0f;
        _button.Disabled = false;
        if (_cooldownBar != null)
          _cooldownBar.Value = 0;
      }
      else
      {
        if (_skill.Skill.Cooldown > 0f && _cooldownBar != null)
        {
          // progress is 100 at the start of cooldown and 0 when finished
          _cooldownBar.Value = _remaining;
        }
      }

      _cooldownLabel.Text = GetCooldownText();
      _cooldownLabel.Visible = _remaining > 0f;
    }
  }

  public void AssignSkill(SkillResource skillResource)
  {
    _skill = skillResource;
    InitializeSkillSlot();
  }

  private void InitializeSkillSlot()
  {
    if (_skill?.Skill is ISkill skill)
    {
      _iconRect.Texture = LoadTexture(_skill.IconPath);
      _cooldownLabel.Text = GetCooldownText();
      if (_keybind != null)
        _keybind.Text = GetActionKeyLabel(inputActionName);
      // initialize cooldown values
      if (_cooldownBar != null)
      {
        _cooldownBar.MinValue = 0;
        _cooldownBar.MaxValue = _skill.Skill.Cooldown;
        _cooldownBar.Value = _remaining;
        _cooldownBar.Step = 0.01f;
      }

      _button.Pressed += OnButtonPressed;
    }
  }

  private void OnButtonPressed()
  {
    if (_skill?.Skill is ISkill skill)
    {
      // Only start cooldown if not already cooling down
      if (_remaining <= 0f)
      {
        _remaining = _skill.Skill.Cooldown > 0f ? _skill.Skill.Cooldown : skill.Cooldown;
        _button.Disabled = _remaining > 0f;
        _cooldownLabel.Text = GetCooldownText();
        _cooldownLabel.Visible = _remaining > 0f;

        // set to full progress when starting cooldown
        _cooldownBar.Value = _remaining;
        // TODO: invoke skill activation logic here (send event / call skill)
      }
    }
  }

  private string GetCooldownText()
  {
    return _remaining > 0f ? Math.Ceiling(_remaining).ToString() : string.Empty;
  }

  private string GetActionKeyLabel(string actionName)
  {
    if (string.IsNullOrWhiteSpace(actionName))
      return string.Empty;

    var events = InputMap.ActionGetEvents(actionName);
    if (events == null || events.Count == 0)
      return string.Empty;

    // Prefer a keyboard/mouse/gamepad button event when available
    foreach (var ev in events)
    {
      switch (ev)
      {
        case InputEventMouseButton mb:
          if (mb.ButtonIndex == MouseButton.Left) return "LMB";
          if (mb.ButtonIndex == MouseButton.Right) return "RMB";
          return ev.AsText();
        case InputEventKey:
        case InputEventJoypadButton:
          return ev.AsText();
      }
    }

    // Fallback to first event's textual representation
    return events[0].AsText();
  }

  private Texture2D LoadTexture(string path)
  {
    return ResourceLoader.Load<Texture2D>(path) ?? throw new InvalidOperationException($"Failed to load texture for skill: {_skill.Skill.Name}");
  }

}
