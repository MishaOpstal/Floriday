'use client'

import Image from 'next/image';
import Link from 'next/link';
import s from '../page.module.css';
import "bootstrap/dist/css/bootstrap-grid.min.css"
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import {useRouter} from 'next/navigation';

export default function LoginPage() {
    const router = useRouter();

    const handleLoginClick = () => {
        router.push('/');
    };


    return (
        <main className={s.main}>
            <title>Login</title>
            <section className={s.card} aria-labelledby="loginTitle">
                <div className={s.logoRow}>
                    <Image
                        src="/LeafBid.svg"
                        alt="Leafbid logo"
                        width={100}
                        height={100}
                        priority
                    />
                </div>

                <h1 id="loginTitle" className={s.title}>Welkom bij Leafbid</h1>

                <Form noValidate className={s.form}>
                    {/* Email */}
                    <Form.Label htmlFor="email" className="form-label">
                        <Form.Control className={s.input} type="email" placeholder="Email"/>
                    </Form.Label>

                    {/* Password */}
                    <Form.Label htmlFor="password" className="form-label">
                        <Form.Control className={s.input} type="password" placeholder="Wachtwoord"/>
                    </Form.Label>

                    {/* Remember me */}
                    <Form.Label className={`form-check-label ${s.check}`} htmlFor="remember me">
                        <Form.Control
                            className={`form-check-input ${s.checkInput} p-0`}
                            type="checkbox"
                            id="remember"
                            name="remember me"
                        />
                        Onthoud mij?
                    </Form.Label>

                    {/* Submit */}
                    <Form.Control as={Button} type="submit" value="Inloggen"
                                  onClick={handleLoginClick}>Inloggen</Form.Control>
                </Form>

                <p className={s.registerLine}>
                    <Link href="/auth/register" className={s.registerLink}>
                        Nog geen account?
                    </Link>
                </p>
            </section>
        </main>
    );
}