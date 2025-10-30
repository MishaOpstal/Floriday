import React from 'react';
import s from "@/components/LoginForm/LoginForm.module.css";

export default function LoginForm() {
    return (
        <div className={s.container}>
            <div className={s.underline}>
                <label className={s.authitem}>Email Adres:</label>
                <input className={s.input} type="email" id="Email" required />
            </div>
            <div className={s.underline}>
                <label className={s.authitem}>Wachtwoord:</label>
                <input className={s.input} type="password" id="Password" required />
            </div>

        </div>
    );
}
