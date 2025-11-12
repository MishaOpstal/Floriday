import InfoVeld from "@/components/infoVeldKlein/infoVeldKlein";
import BigInfoVeld from "@/components/veilingInfo/veilingInfo";
import Header from "@/components/header/header";
import VeilingKlok from "@/components/veilingKlok/veilingKlok";
import 'bootstrap/dist/css/bootstrap.min.css';
import s from "./page.module.css"


export default function Profile() {
    return (
        <> <Header returnOption={true} />
        <main className={s.main}>
            <div className={s.links}>
                {/*<VeilingKlok bedrag={"$5000"} locatie={"Zuid Holland"} tijd={"1:10"}/>*/}
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
