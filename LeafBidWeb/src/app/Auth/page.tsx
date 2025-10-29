import Head from "next/head";
import Header from "@/components/header/header";
import LoginS from "@/components/LoginForm/LoginForm.module.css";
import RegisterS from "@/components/RegisterForm/RegisterForm.module.css";
import s from  "@/app/Auth/page.module.css"

import React from "react";

export default function Login(){
    const [login, setLogin] = React.useState(true);

    function handleChange(event: React.ChangeEvent<HTMLInputElement>){
        setLogin(event.currentTarget.value === "login");
    }

    return (
      <>
        <input value={login} onChange={handleChange} />
          <p>Value: {login}</p>
      </>
    );
}