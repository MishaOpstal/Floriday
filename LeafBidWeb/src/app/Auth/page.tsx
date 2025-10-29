import Head from "next/head";
import Header from "@/components/header/header";
import LoginS from "@/components/LoginForm/LoginForm.module.css";
import RegisterS from "@/components/RegisterForm/RegisterForm.module.css";

import s from  "@/app/Auth/page.module.css"
import React from "react";
import Image from "next/image";

export default function Login(){
    return(

        <div className={s.startFrame}>
            <div className={s.frame}>
                <div className={s.image}>
                </div>
                <div className={s.loginRegister}>

                </div>
            </div>
        </div>
    );
}