import s from "@/app/Auth/Register/page.module.css"
import React from "react";
import RegisterForm from "@/components/RegisterForm/RegisterForm";
import Link from "next/link";

export default function Login(){
    return(
        <main className={s.main}>
            <div className={s.startScreen}>
                <div className={s.startFrame}>
                    <div className={s.image}></div>
                    <div className={s.loginRegister}>
                        <div className={s.selector}>
                            <Link href="/Auth/Login">
                                <div className={s.login} >
                                    <h3>Login</h3>
                                </div>
                            </Link>
                            <div className={s.register}>
                                <h3>Register</h3>
                            </div>
                        </div>
                        <div className={s.inputs}><RegisterForm /></div>
                        <div className={s.logo}>
                            <div className={s.innerLogo}>
                                <div className={s.logoImage}>
                                </div>
                                <div className={s.logoText}>
                                    <h1>LeafBid</h1>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    );
}