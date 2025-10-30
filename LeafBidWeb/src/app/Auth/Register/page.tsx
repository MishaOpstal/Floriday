import Head from "next/head";
import Header from "@/components/header/header";
import LoginS from "@/components/LoginForm/LoginForm.module.css";
import RegisterS from "@/components/RegisterForm/RegisterForm.module.css";

import s from "@/app/Auth/register/page.module.css"
import React from "react";
import Image from "next/image";
import RegisterForm from "@/components/RegisterForm/RegisterForm";

export default function Login(){
    return(
        <main className={s.main}>
            <div className={s.startScreen}>
                <div className={s.startFrame}>
                    <div className={s.image}></div>
                    <div className={s.loginRegister}>
                        <div className={s.selector}>
                            <div className={s.login}>
                                <h3>Login</h3>
                            </div>
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