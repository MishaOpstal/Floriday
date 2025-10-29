import Header from "@/components/header/header";
import s from './layout.module.css';
import React from "react";

export default function RootLayout({
                                       children,
                                   }: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <>
            <Header />
            <main className={s.main}>{children}</main>
        </>
    );
}