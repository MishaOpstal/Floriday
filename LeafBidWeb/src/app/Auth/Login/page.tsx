import Image from 'next/image';
import Link from 'next/link';
import s from './page.module.css';
import "bootstrap/dist/css/bootstrap-grid.min.css"
import Form from "react-bootstrap/Form";

export default function LoginPage() {
    return (
        <main className={s.main}>
            <section className={s.card} aria-labelledby="loginTitle">
                <div className={s.logoRow}>
                    <Image
                        src="/LeafBid.svg"
                        alt="Leafbid logo"
                        width={64}
                        height={64}
                        priority
                    />
                </div>

                <h1 id="loginTitle" className={s.title}>Welkom bij Leafbid</h1>

                <form noValidate>
                    {/* Email */}
                    <div >
                        <div className="mb-3">
                            <label htmlFor="email" className="form-label"></label>
                            <Form.Control className={s.input} type="email" placeholder="Email"/>
                        </div>
                    </div>

                    {/* Password */}
                    <div>
                        <div className="mb-3">
                            <label htmlFor="password" className="form-label"></label>
                            <Form.Control className={s.input} type="password" placeholder="Wachtwoord"/>
                        </div>
                    </div>

                    {/* Remember me */}
                    <div className={`form-check ${s.check}`}> {/* keep Bootstrap semantics, add spacing via CSS module */}
                        <input
                            className={`form-check-input bg-white ${s.checkInput}`}
                            type="checkbox"
                            id="remember"
                            name="remember"
                        />
                        <label className="form-check-label" htmlFor="remember">
                            Onthoud mij?
                        </label>
                    </div>

                    {/* Submit */}
                    <button type="button" className="btn btn-success">Inloggen</button>
                </form>

                <p className={s.registerLine}>
                    <Link href="/Auth/Register" className={s.registerLink}>
                        Nog geen account? Registreer
                    </Link>
                </p>
            </section>
        </main>
    );
}