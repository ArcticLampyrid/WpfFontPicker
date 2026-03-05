# syntax=docker/dockerfile:1.7

FROM archlinux:latest AS runtime

RUN --mount=type=cache,target=/var/cache/pacman/pkg \
    pacman -Syu --noconfirm && \
    pacman -S --noconfirm --needed \
    bash \
    rsync \
    ca-certificates \
    curl \
    openssl \
    unzip \
    xorg-server-xvfb \
    xorg-xset \
    xdotool \
    imagemagick \
    herbstluftwm \
    dbus \
    dotnet-sdk \
    mesa \
    libglvnd \
    ttf-dejavu \
    ttf-liberation \
    noto-fonts \
    noto-fonts-cjk

RUN groupadd -g 1000 runner && \
    useradd -m -u 1000 -s /bin/bash -g runner runner && \
    mkdir -p /work /src /output && \
    chown -R runner:runner /work /src /output /home/runner

USER runner

ENV HOME=/home/runner \
    XDG_RUNTIME_DIR=/tmp/runtime-runner \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=1 \
    DOTNET_CLI_HOME=/home/runner/.dotnet \
    NUGET_PACKAGES=/home/runner/.nuget/packages \
    DOTNET_ROLL_FORWARD=Major \
    LIBGL_ALWAYS_SOFTWARE=1

RUN mkdir -p "$XDG_RUNTIME_DIR" "$DOTNET_CLI_HOME" "$NUGET_PACKAGES" /home/runner/.nuget/NuGet && \
    chmod 700 "$XDG_RUNTIME_DIR"

FROM runtime AS builder
COPY --chown=runner:runner . /src
RUN --mount=type=cache,uid=1000,gid=1000,target=/home/runner/.nuget/packages \
    dotnet build -c Release /src/ALampy.XamlFontPicker.Avalonia.Sample/ALampy.XamlFontPicker.Avalonia.Sample.csproj

FROM runtime AS final
COPY --chown=runner:runner --from=builder /src/ALampy.XamlFontPicker.Avalonia.Sample/bin/Release/net8.0 /app
COPY --chown=runner:runner ./Utils/AvaloniaSampleScreenshot/entrypoint-in-container.sh /app/entrypoint-in-container.sh
RUN chmod +x /app/entrypoint-in-container.sh
ENTRYPOINT ["/app/entrypoint-in-container.sh"]
