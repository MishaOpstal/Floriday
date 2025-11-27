"use client";

import { useEffect } from "react";
import { refreshToken } from "@/app/auth/utils/RefreshToken";

export default function AuthBootstrapper() {
    useEffect(() => {
        let timer: number | undefined;

        const schedule = async () => {
            try {
                await refreshToken();
            } catch {
                return;
            }

            const skewMs = 30_000;
            const expiresAt = window.accessTokenExpiresAt;

            if (!expiresAt) return;

            const delay = Math.max(0, expiresAt - Date.now() - skewMs);
            timer = window.setTimeout(async () => {
                try {
                    await refreshToken();
                } finally {
                    schedule();
                }
            }, delay);
        };

        schedule();

        return () => {
            if (timer) {
                window.clearTimeout(timer);
            }
        };
    }, []);

    return null;
}
