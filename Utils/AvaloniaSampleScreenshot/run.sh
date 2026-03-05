#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

IMAGE_NAME="${IMAGE_NAME:-xaml-font-picker-avalonia-sample-screenshot:latest}"
OUTPUT_DIR="${OUTPUT_DIR:-/tmp/xaml-font-picker-avalonia-sample-screenshot}"

mkdir -p "${OUTPUT_DIR}"
chmod 1777 "${OUTPUT_DIR}"

BUILD_ARGS=(
  --load
  --file "${SCRIPT_DIR}/Dockerfile"
  --tag "${IMAGE_NAME}"
  -f "${SCRIPT_DIR}/Dockerfile"
  "${REPO_ROOT}"
)

echo "[1/2] Building image: ${IMAGE_NAME}"
BUILDX_LOG=$(mktemp "/tmp/buildx-log-XXXXXX.txt")
if ! docker buildx build "${BUILD_ARGS[@]}" --progress=plain >"${BUILDX_LOG}" 2>&1; then
  echo "Error: Docker build failed. See ${BUILDX_LOG} for details."
  echo "Please manually remove it after reviewing the log."
  exit 1
fi
rm -f "${BUILDX_LOG}"

echo "[2/2] Running screenshot job"
docker run --rm \
  --init \
  -t \
  -v "${OUTPUT_DIR}:/output" \
  "${IMAGE_NAME}"

echo "截图已保存。"
find "${OUTPUT_DIR}" -type f -print
