import s from "./header.module.css";
import Image from "next/image";
import Link from "next/link";
import {Moon, MoonFill} from "react-bootstrap-icons";
import ThemeInitializer ,{toggleTheme} from './darkmode'

interface HeaderProps {
    returnOption?: boolean;
}


export default function Header({ returnOption = false }: HeaderProps) {
    return (
        <header>
            <ThemeInitializer />
            {
            }
            <div className={s.logoWrapper}>
            <Image
                src="/LeafBid.svg"
                alt="LeafBid Logo"
                fill
                style={{objectFit: "contain"}}
                priority
            />
            </div>
            <nav aria-label="main navigation">
            {returnOption && (
                <Link href="/" className={s.link}>
                    Terug
                </Link>
            )}

                <div className={s.clickables}>
                    <Link
                        href="/Auth/Login"
                        className={s.link}
                    >
                        Uitloggen
                    </Link>
                    <MoonFill
                        onClick={toggleTheme}
                        className={s.themeToggle}

                    />
                </div>
            </nav>
        </header>
    );
}

