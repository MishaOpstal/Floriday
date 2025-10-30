import React from 'react';
import s from "@/components/RegisterForm/RegisterForm.module.css";

export default function RegisterForm() {
    return (
        <div className={s.container}>
            <div className={s.underline}>
                <label className={s.authitem}>Naam:</label>
                <input className={s.input} type="text" id="name" required />
            </div>
            <div className={s.underline}>
                <label className={s.authitem}>Email Adres:</label>
                <input className={s.input} type="email" id="Email" required />
            </div>
            <div className={s.underline}>
                <label className={s.authitem}>Wachtwoord:</label>
                <input className={s.input} type="password" id="Password" required />
            </div>

            <div className={s.underline}>
                <label className={s.authitem}>Wachtwoord:</label>
                <input className={s.input} type="password" id="PasswordCheck" required />
            </div>
        </div>
    );
}