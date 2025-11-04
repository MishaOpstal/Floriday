import 'bootstrap/dist/css/bootstrap.min.css';
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
                    <InfoVeld naam="Roos" prijs={"$10000"} plaatje="bloem.png"/>
                    <InfoVeld naam="Roos" prijs={"$10000"} plaatje="bloem.png"/>
                    <InfoVeld naam="Roos" prijs={"$10000"} plaatje="bloem.png"/>
                </div>
            </div>
            <div className={s.infoblok}>
                <BigInfoVeld naam="Naam"
                             prijs={10000}
                             plaatje="bloem.png"
                             info="De aardappelplant vormt ondergronds eetbare knollen. Ze groeit uit pootaardappelen, houdt van losse grond en levert een voedzaam, veelzijdig gewas."
                             oogst="10 april"
                             leverancier="pieter"
                             regio="zuid holland"
                             aantal={100}
                />
            </div>
        </main>
        </>
    );
}
