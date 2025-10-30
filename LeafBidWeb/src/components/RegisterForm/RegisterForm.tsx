import React from 'react';
import s from "@/components/RegisterForm/RegisterForm.module.css";
import Link from "next/link";

export default function RegisterForm() {
    return (
        <div className={s.container}>
            <div className={s.underline}>
                <label className={`${s.authitem} ${s.srOnly}`} htmlFor="name">Naam:</label>
                <input className={s.input} type="text" id="name" placeholder="Naam" required />
            </div>
            <div className={s.underline}>
                <label className={`${s.authitem} ${s.srOnly}`} htmlFor="Email">Email Adres:</label>
                <input className={s.input} type="email" id="Email" placeholder="Email Adres" required />
            </div>
            <div className={s.underline}>
                <label className={`${s.authitem} ${s.srOnly}`} htmlFor="Password">Wachtwoord:</label>
                <input className={s.input} type="password" id="Password" placeholder="Wachtwoord" required />
            </div>

            <div className={s.underline}>
                <label className={`${s.authitem} ${s.srOnly}`} htmlFor="PasswordCheck">Wachtwoord:</label>
                <input className={s.input} type="password" id="PasswordCheck" placeholder="Wachtwoord" required />
            </div>

            <div className={s.signInButton}>
                <Link href="/">
                    <input className={s.buttonStyle} type="button" id="SignUpButton" value="Registreren" />
                </Link>
            </div>
        </div>
    );
}