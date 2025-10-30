import Head from "next/head";
import Header from "@/components/header/header";
import LoginS from "@/components/LoginForm/LoginForm.module.css";
import RegisterS from "@/components/RegisterForm/RegisterForm.module.css";

import s from "@/app/Auth/Login/page.module.css"
import React from "react";
import Image from "next/image";

export default function Login(){
    return(
        <main className={s.main}>
            <div className={s.startScreen}>
                <div className={s.startFrame}>
                    <div className={s.image}></div>
                    <div className={s.loginRegister}>
                        <h1>bla bla bla</h1>
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