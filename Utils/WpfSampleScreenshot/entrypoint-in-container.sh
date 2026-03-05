#!/usr/bin/env bash
set -euo pipefail

SOURCE_DIR=/src
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
  wineserver -k >/dev/null 2>&1 || true
}
trap cleanup EXIT

Xvfb "$DISPLAY" -screen 0 1920x1080x24 -nolisten tcp >/tmp/xvfb.log 2>&1 &
XVFB_PID=$!

sleep 1

herbstluftwm --locked >/tmp/wm.log 2>&1 &
WM_PID=$!

sleep 1

# Configure herbstluftwm for tiling: use a grid layout so all windows are visible
herbstclient detect_monitors
herbstclient set frame_gap 0
herbstclient set window_gap 0
herbstclient set smart_frame_surroundings off
herbstclient set smart_window_surroundings off
# Use "grid" layout so multiple windows tile side-by-side / in a grid
herbstclient set default_frame_layout grid
herbstclient set_layout grid

wine "/app/ALampy.XamlFontPicker.WPF.Sample.exe" --open-dialog-on-startup >/tmp/app.log 2>&1 &
APP_PID=$!

timeout 120 xdotool search --sync --name "MainWindow" >/dev/null
if [ $? -ne 0 ]; then
  echo "Warning: MainWindow did not appear within timeout. "
fi
timeout 120 xdotool search --sync --name "Choose Font" >/dev/null
if [ $? -ne 0 ]; then
  echo "Warning: Choose Font dialog did not appear within timeout. "
fi

sleep 5

import -display "$DISPLAY" -window root "$OUTPUT_DIR/full-screen.png"
