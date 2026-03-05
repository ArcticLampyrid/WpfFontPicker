#!/usr/bin/env bash
set -euo pipefail

OUTPUT_DIR=/output
export DISPLAY=:99

mkdir -p "$OUTPUT_DIR"

cleanup() {
  set +e
  if [[ -n "${APP_PID:-}" ]]; then
    kill "$APP_PID" 2>/dev/null || true
  fi
  if [[ -n "${WM_PID:-}" ]]; then
    kill "$WM_PID" 2>/dev/null || true
  fi
  if [[ -n "${XVFB_PID:-}" ]]; then
    kill "$XVFB_PID" 2>/dev/null || true
  fi
}
trap cleanup EXIT

Xvfb "$DISPLAY" -screen 0 1920x1080x24 -nolisten tcp >/tmp/xvfb.log 2>&1 &
XVFB_PID=$!

sleep 1

herbstluftwm --locked >/tmp/wm.log 2>&1 &
WM_PID=$!

sleep 1

herbstclient detect_monitors
herbstclient set frame_gap 0
herbstclient set window_gap 0
herbstclient set smart_frame_surroundings off
herbstclient set smart_window_surroundings off
herbstclient set default_frame_layout grid
herbstclient set_layout grid
# Disable floating for all new windows so layout is fully tiled.
herbstclient rule floating=off

dotnet /app/ALampy.XamlFontPicker.Avalonia.Sample.dll --open-dialog-on-startup >/tmp/app.log 2>&1 &
APP_PID=$!

if ! timeout 120 xdotool search --sync --name "MainWindow" >/dev/null; then
  if ! timeout 10 xdotool search --sync --name "Avalonia Sample - FontPicker" >/dev/null; then
    echo "Warning: main window did not appear within timeout."
    tail -n 80 /tmp/app.log || true
  fi
fi

if ! timeout 120 xdotool search --sync --name "Choose Font" >/dev/null; then
  echo "Warning: font dialog did not appear within timeout."
  tail -n 80 /tmp/app.log || true
fi

sleep 5

import -display "$DISPLAY" -window root "$OUTPUT_DIR/full-screen.png"
