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
    wine \
    wine-mono \
    wine-gecko \
    cabextract \
    p7zip \
    ttf-dejavu \
    ttf-liberation \
    noto-fonts \
    noto-fonts-cjk

RUN curl https://raw.githubusercontent.com/Winetricks/winetricks/master/src/winetricks -o /usr/local/bin/winetricks && \
    chmod a+x /usr/local/bin/winetricks

# Trust VeriSign Universal Root CA, which is required for NuGet certificate validation.
# See also https://bugs.winehq.org/show_bug.cgi?id=59360
RUN set -euo pipefail && \
    cert_der=/tmp/verisign-universal-root.der && \
    cert_pem=/tmp/verisign-universal-root.pem && \
    url="http://www.download.windowsupdate.com/msdownload/update/v3/static/trustedr/en/3679CA35668772304D30A5FB873B0FA77BB70D54.crt" && \
    if curl -C - -fL --retry 5 --retry-delay 5 "$url" -o "$cert_der"; then \
    echo "Downloaded VeriSign root certificate from: $url"; \
    fi; \
    test -s "$cert_der" && \
    openssl x509 -inform DER -in "$cert_der" -out "$cert_pem" && \
    install -D -m 0644 "$cert_pem" /etc/ca-certificates/trust-source/anchors/verisign-universal-root-ca.pem && \
    update-ca-trust extract

RUN groupadd -g 1000 runner && \
    useradd -m -u 1000 -s /bin/bash -g runner runner && \
    mkdir -p /work /src /output && \
    chown -R runner:runner /work /src /output /home/runner

USER runner

ENV HOME=/home/runner \
    WINEDEBUG=-all \
    WINEPREFIX=/home/runner/.wine \
    XDG_RUNTIME_DIR=/tmp/runtime-runner \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=1

RUN --mount=type=cache,uid=1000,gid=1000,target=/tmp/win-dotnet-cache \
    mkdir -p "$XDG_RUNTIME_DIR" && chmod 700 "$XDG_RUNTIME_DIR" && \
    xvfb-run --auto-servernum --server-args="-screen 0 1920x1080x24" -- bash -c "wineboot -u && wineserver -w" && \
    winetricks -q corefonts && \
    curl -C - -fL --retry 5 --retry-delay 5 "https://builds.dotnet.microsoft.com/dotnet/Sdk/3.1.426/dotnet-sdk-3.1.426-win-x64.exe" -o /tmp/win-dotnet-cache/dotnet-sdk-3.1.426-win-x64.exe && \ 
    xvfb-run --auto-servernum --server-args="-screen 0 1920x1080x24" -- wine /tmp/win-dotnet-cache/dotnet-sdk-3.1.426-win-x64.exe /quiet /norestart


FROM runtime AS builder
COPY --chown=runner:runner . /src
RUN --mount=type=cache,uid=1000,gid=1000,target=/tmp/nuget-cache \
    rm -rf ${WINEPREFIX}/drive_c/users/runner/.nuget && \
    ln -s /tmp/nuget-cache ${WINEPREFIX}/drive_c/users/runner/.nuget && \
    xvfb-run --auto-servernum --server-args="-screen 0 1920x1080x24" -- wine "C:\Program Files\dotnet\dotnet.exe" build -c Release -f netcoreapp3.1 "Z:\src\ALampy.XamlFontPicker.WPF.Sample\ALampy.XamlFontPicker.WPF.Sample.csproj"

FROM runtime AS final
COPY --chown=runner:runner --from=builder /src/ALampy.XamlFontPicker.WPF.Sample/bin/Release/netcoreapp3.1 /app
COPY --chown=runner:runner ./Utils/WpfSampleScreenshot/entrypoint-in-container.sh /app/entrypoint-in-container.sh
RUN chmod +x /app/entrypoint-in-container.sh
ENTRYPOINT ["/app/entrypoint-in-container.sh"]

