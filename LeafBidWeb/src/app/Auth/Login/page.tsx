import s from "@/app/Auth/Login/page.module.css"
import React from "react";
import LoginForm from "@/components/LoginForm/LoginForm";

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
                       <div className={s.register}><h3>Register</h3></div>
                   </div>
                   <div className={s.inputs}><LoginForm /></div>
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