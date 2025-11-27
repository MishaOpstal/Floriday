export async function refreshToken() {
    const res = await fetch("/auth/refresh", {
        method: "POST",
        credentials: "include",
    });

    if (!res.ok) throw new Error("Refresh mislukt");

    const data = await res.json();

    window.accessToken = data.accessToken;

    if (typeof data.expiresIn === "number") {
        window.accessTokenExpiresAt = Date.now() + data.expiresIn * 1000;
    }
}
