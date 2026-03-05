#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

IMAGE_NAME="${IMAGE_NAME:-xaml-font-picker-wpf-sample-screenshot:latest}"
OUTPUT_DIR="${OUTPUT_DIR:-/tmp/xaml-font-picker-wpf-sample-screenshot}"
VERBOSE="${VERBOSE:-false}"

print_usage() {
  cat <<'USAGE'
Usage: run.sh [options]

Options:
  --output-dir <path>   Directory where screenshots are written.
  --verbose             Print full docker build logs directly.
  -h, --help            Show this help message.
USAGE
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --output-dir)
      [[ $# -ge 2 ]] || { echo "Error: --output-dir requires a value." >&2; exit 2; }
      OUTPUT_DIR="$2"
      shift 2
      ;;
    --output-dir=*)
      OUTPUT_DIR="${1#*=}"
      shift
      ;;
    --verbose)
      VERBOSE=true
      shift
      ;;
    -h|--help)
      print_usage
      exit 0
      ;;
    *)
      echo "Error: Unknown argument: $1" >&2
      print_usage >&2
      exit 2
      ;;
  esac
done

mkdir -p "${OUTPUT_DIR}"
chmod 1777 "${OUTPUT_DIR}"

BUILD_ARGS=(
  --load
  --file "${SCRIPT_DIR}/Dockerfile"
  --tag "${IMAGE_NAME}"
  "${REPO_ROOT}"
)

echo "[1/2] Building image: ${IMAGE_NAME}"
if [[ "${VERBOSE}" == "true" ]]; then
  docker buildx build "${BUILD_ARGS[@]}" --progress=plain
else
  BUILDX_LOG=$(mktemp "/tmp/buildx-log-XXXXXX.txt")
  if ! docker buildx build "${BUILD_ARGS[@]}" --progress=plain >"${BUILDX_LOG}" 2>&1; then
    echo "Error: Docker build failed."
    echo "Hint: run with --verbose to print full logs directly."
    echo "Build log: ${BUILDX_LOG}"
    exit 1
  fi
  rm -f "${BUILDX_LOG}"
fi

echo "[2/2] Running screenshot job"
docker run --rm \
  --init \
  -v "${OUTPUT_DIR}:/output" \
  "${IMAGE_NAME}"

echo "截图已保存。"
find "${OUTPUT_DIR}" -type f -print
