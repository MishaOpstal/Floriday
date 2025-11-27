export async function refreshToken() {
    const res = await fetch("http://localhost:5001/auth/refresh", {
        method: "POST",
        credentials: "include",
    });

    if (!res.ok) throw new Error("Refresh mislukt");

    const data = await res.json();

    if (!data.accessToken) throw new Error("Geen token");
    window.accessToken = data.accessToken;
    localStorage.setItem("bearerToken", data.accessToken)

    if (typeof data.expiresIn === "number") {
        window.accessTokenExpiresAt = Date.now() + data.expiresIn * 1000;
    }
}
