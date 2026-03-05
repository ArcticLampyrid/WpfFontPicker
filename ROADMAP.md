# ROADMAP — Avalonia Port for ALampy.XamlFontPicker

> Goal: deliver `ALampy.XamlFontPicker.Avalonia` (library + sample) while keeping `ALampy.XamlFontPicker.WPF` stable and releasable.

## 0) Working Agreement (OpenCode + Koyuki)

- OpenCode (大姐姐) implements each scoped task.
- Koyuki reviews every patch locally (build/run/diff/API consistency), applies fixes if needed, then commits/pushes.
- Merge strategy: small vertical slices, each slice must build and be testable.
- Definition of done per slice:
  - `dotnet build` passes for affected projects.
  - sample app can run for the new functionality.
  - no regression in current WPF project.

---

## 1) Milestones

### M1 — Avalonia Project Bootstrap (MVP foundation) [S]

- [ ] Add `ALampy.XamlFontPicker.Avalonia/ALampy.XamlFontPicker.Avalonia.csproj`
- [ ] Add `ALampy.XamlFontPicker.Avalonia.Sample/ALampy.XamlFontPicker.Avalonia.Sample.csproj`
- [ ] Wire new projects into `WpfFontPicker.sln`
- [ ] Configure Avalonia packages (11.x), fluent theme in sample
- [ ] Ensure clean build for solution (WPF + Avalonia projects)

**Deliverable:** Empty but runnable Avalonia sample window.

---

### M2 — Core Model & ViewModel Port [S]

- [ ] Port `PickedFontInfo` to Avalonia media types
- [ ] Port `FontSizeOption`
- [ ] Port `FontDialogViewModel` core logic
- [ ] Replace font source with `FontManager.Current.SystemFonts`
- [ ] Keep point/pixel conversion behavior aligned with WPF implementation

**Deliverable:** ViewModel unit-testable and usable by Avalonia UI.

---

### M3 — FontDialog UI (Avalonia) [M]

- [ ] Implement `FontDialog.axaml` 3-column layout (family/typeface/size)
- [ ] Add search box behavior parity with WPF:
  - [ ] prefer current selected item when matching
  - [ ] otherwise jump to first matching item
  - [ ] no-op when no match
  - [ ] distinguish user typing vs selection sync behavior
- [ ] Implement loaded-time `ScrollIntoView` for selected family
- [ ] Hook OK/Cancel and return selected `PickedFontInfo`

**Deliverable:** Font dialog feature parity with current WPF UX baseline.

---

### M4 — FontPicker Control (Avalonia) [M]

- [ ] Implement `FontPicker.axaml` + code-behind
- [ ] Add `SelectedFontInfo` property (Avalonia styled property)
- [ ] Add API equivalent of `OpenFontDialog()`
- [ ] Ensure binding updates and preview behavior

**Deliverable:** Embeddable Avalonia control with dialog interaction.

---

### M5 — Localization & Resource Strategy [S]

- [ ] Establish Avalonia resource dictionary strings (`Resources/Strings*.axaml`)
- [ ] Port language lookup converter / helper
- [ ] Keep EN + zh-CN parity with WPF labels

**Deliverable:** localized UI text in Avalonia dialog.

---

### M6 — Sample Polish + Packaging [M]

- [ ] Build a realistic sample screen (button + FontPicker + result preview)
- [ ] Update README usage section for Avalonia
- [ ] Decide package naming/versioning (`ALampy.XamlFontPicker.Avalonia`)
- [ ] Add release checklist for NuGet publish

**Deliverable:** documented, demoable first Avalonia release candidate.

---

## 2) Task Sequencing (Execution Queue)

1. **Bootstrap projects (M1)**
2. **Port model/viewmodel (M2)**
3. **Implement dialog UX parity (M3)**
4. **Implement reusable control API (M4)**
5. **Localization pass (M5)**
6. **Docs + package prep (M6)**

---

## 3) Risk Register

- **Font API differences (WPF vs Avalonia):** family names/typeface availability may differ by platform.
  - Mitigation: normalize display name helper + fallback chain.
- **Cross-platform rendering variance:** line-height/preview may not match WPF exactly.
  - Mitigation: define behavior-oriented tests instead of pixel-perfect assertions.
- **Search UX regressions:** typing-sync logic is easy to break.
  - Mitigation: isolate logic and keep an explicit scenario checklist.
- **Packaging fragmentation:** old WPF naming vs new Avalonia naming confusion.
  - Mitigation: explicit package table in README.

---

## 4) Verification Checklist per Milestone

- [ ] `dotnet build WpfFontPicker.sln /p:EnableWindowsTargeting=true`
- [ ] run WPF screenshot utility (regression guard)
- [ ] run Avalonia sample manually (for new milestone)
- [ ] review public API names for consistency (`SelectedFontInfo`, `OpenFontDialog`)
- [ ] commit message uses conventional commits

---

## 5) Current Status

- [x] Roadmap drafted
- [ ] M1 in progress
- [ ] M2 pending
- [ ] M3 pending
- [ ] M4 pending
- [ ] M5 pending
- [ ] M6 pending
