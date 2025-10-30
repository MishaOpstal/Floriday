import s from "./header.module.css";
import Image from "next/image";
import Link from "next/link";

interface HeaderProps {
    returnOption?: boolean;
}

export default function Header({ returnOption = false }: HeaderProps) {
    return (
        <header>
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
                <Link
                    href="/Auth/Login"
                    className={s.link}
                >
                    Uitloggen
                </Link>

            {returnOption && (
                <Link href="/" className={s.link}>
                    Terug
                </Link>
            )}
            </nav>
        </header>
    );
}

