import Image from 'next/image';
import Link from 'next/link';
import s from './page.module.css';
import "bootstrap/dist/css/bootstrap-grid.min.css"

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
                        <label htmlFor="email"></label>
                        <div className="input-group mb-3">
                           <span className="input-group-text" id="basic-addon1"></span>
                           <input type="text" className="form-control" placeholder="Email" aria-label="Email" aria-describedby="basic-addon1"/>
                        </div>
                    </div>

                    {/* Password */}
                    <div>
                        <label htmlFor="password"></label>
                        <div className="input-group mb-3">
                            <span className="input-group-text" id="basic-addon1"></span>
                            <input type="password" className="form-control" placeholder="Wachtwoord" aria-label="Wachtwoord" aria-describedby="basic-addon1"/>
                        </div>
                    </div>

                    {/* Remember me */}
                    <div className={`form-check ${s.check}`}> {/* keep Bootstrap semantics, add spacing via CSS module */}
                        <input
                            className="form-check-input"
                            type="checkbox"
                            id="remember"
                            name="remember"
                        />
                        <label className="form-check-label" htmlFor="remember">
                            Onthoud mij?
                        </label>
                    </div>

                    {/* Submit */}
                    <button type="submit" className="btn btn-success w-100">
                        Inloggen
                    </button>
                </form>

                <p className={s.registerLine}>
                    Nog geen account?{' '}
                    <Link href="/Auth/Register" className={s.registerLink}>
                        Registreer
                    </Link>
                </p>
            </section>
        </main>
    );
}