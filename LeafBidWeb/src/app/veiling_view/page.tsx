import InfoVeld from "@/components/infoVeldKlein/infoVeldKlein";
import BigInfoVeld from "@/components/veilingInfo/veilingInfo";
import Header from "@/components/header/header";
import s from "./page.module.css"

export default function Profile() {
    return (
        <> <Header returnOption={true} />
        <main className={s.main}>
            <div className={s.links}>
                <img className={s.plaatje} src="/bloem.png" alt="placeholder" />
                <div className={s.tekstblokken}>
                    <InfoVeld naam="Naam" prijs={10000} plaatje="bloem.png"/>
                    <InfoVeld naam="Naam" prijs={10000} plaatje="bloem.png"/>
                    <InfoVeld naam="Naam" prijs={10000} plaatje="bloem.png"/>
                </div>
            </div>
            <div className={s.infoblok}>
                <BigInfoVeld naam="Naam" prijs={10000} plaatje="bloem.png" duur="1 uur" info="epic bloemen" oogst="10 april" leverancier="pieter" regio="zuid holland" aantal={100}/>
            </div>
        </main>
        </>
    );
}
