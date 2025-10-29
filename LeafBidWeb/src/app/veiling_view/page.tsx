import InfoVeld from "@/components/infoVeldKlein/infoVeldKlein";
import BigInfoVeld from "@/components/veilingInfo/veilingInfo";

export default function Profile() {
    return (
        <main>
            {/*<InfoVeld naam="Naam" prijs={10000} plaatje="bloem.png"/>*/}
            <BigInfoVeld naam="Naam" prijs={10000} plaatje="bloem.png" duur="1 uur" info="epic bloemen" oogst="10 april" leverancier="pieter" regio="zuid holland" aantal={100}/>

        </main>
    );
}
