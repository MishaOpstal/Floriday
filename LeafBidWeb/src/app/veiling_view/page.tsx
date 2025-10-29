import InfoVeld from "@/components/infoVeldKlein/infoVeldKlein";

export default function Profile() {
    return (
        <main>
            <InfoVeld label="Naam" value="Nick poots" />
            <InfoVeld label="E-mail" value="nick@example.com" />
            <InfoVeld label="Locatie" value="Den Haag, Nederland" />
        </main>
    );
}
