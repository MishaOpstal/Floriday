import s from "./header.module.css";
import Image from "next/image";
import Link from "next/link";
import {MoonFill, Sun} from "react-bootstrap-icons";
import ThemeInitializer, {getTheme, toggleTheme} from './theme'
import {useEffect, useState} from "react";

interface HeaderProps {
    returnOption?: boolean;
}


export default function Header({ returnOption = false }: HeaderProps) {
    const [theme, setTheme] = useState<"dark" | "light">("light");

    useEffect(() => {
        // sync with the initializer/localStorage
        setTheme(getTheme());
    }, []);

    const onToggleTheme = () => {
        toggleTheme();
        // toggleTheme updates localStorage + data-theme, so we can re-read
        setTheme(getTheme());
    };

    return (
        <header>
            <ThemeInitializer />
            <div className={s.logoWrapper}>
            <Image
                src="/LeafBid.svg"
                alt="LeafBid Logo"
                fill
                style={{objectFit: "contain"}}
                priority
            />
            </div>
            <nav aria-label="main navigation" className="user-select-none">
            {returnOption && (
                <Link href="/" className={s.link}>
                    Terug
                </Link>
            )}

                <div className={s.clickables}>
                    <Link href="/auth/login" className={s.link}>
                        Uitloggen
                    </Link>

                    {theme === "light" ? (
                        <Sun title="Switch to dark mode" onClick={onToggleTheme} className={s.themeToggle} />
                    ) : (
                        <MoonFill title="Switch to light mode" onClick={onToggleTheme} className={s.themeToggle} />
                    )}
                </div>
            </nav>
        </header>
    );
}

