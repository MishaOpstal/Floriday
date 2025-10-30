import React from 'react';
import s from "@/components/LoginForm/LoginForm.module.css";
import Link from "next/link";

export default function LoginForm() {
    return (
        <div className={s.container}>
            <div className={s.underline}>
                <label className={`${s.authitem} ${s.srOnly}`} htmlFor="Email">Email Adres:</label>
                <input className={s.input} type="email" id="Email" placeholder="Email Adres" required />
            </div>
            <div className={s.underline}>
                <label className={`${s.authitem} ${s.srOnly}`} htmlFor="Password">Wachtwoord:</label>
                <input className={s.input} type="password" id="Password" placeholder="Wachtwoord" required />
            </div>
            <div className={s.logInButton}>
                <Link href="/">
                    <input className={s.buttonStyle} type="button" id="SignUpButton" value="Inloggen" />
                </Link>
            </div>
        </div>
    );
}
