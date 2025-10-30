import styles from '../page.module.css';
import Header from "@/components/header/header";
import Knop from "@/components/knop/knop";
import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";

export default function Home() {
    return (
        <>
            <Header></Header>
            <div className={styles.page}>

                <div className={styles.main}>

                    <DashboardPanel
                        title="Tulpenmix 'Lentezon'"
                        imageSrc="/images/bloem.png"
                        veilingduur="2 min 15 sec"
                        totaalprijs="€ 12,30"
                        kloklocatie="Klok 1 - Hal A"
                    >
                        <Knop label="Aanpassen✏️" to="" />
                        <Knop label="Verwijderen🗑️" to="" />
                    </DashboardPanel>

                    <DashboardPanel
                        title="Rozenpakket 'Romance'"
                        imageSrc="/images/bloem.png"
                        veilingduur="1 min 45 sec"
                        totaalprijs="€ 18,90"
                        kloklocatie="Klok 2 - Hal B"
                    >
                        <Knop label="Aanpassen✏️" to="" />
                        <Knop label="Verwijderen🗑️" to="" />
                    </DashboardPanel>

                    <DashboardPanel
                        title="Zomerboeket 'Veldkracht'"
                        imageSrc="/images/bloem.png"
                        veilingduur="3 min 00 sec"
                        totaalprijs="€ 15,75"
                        kloklocatie="Klok 3 - Hal C"
                    >
                        <Knop label="Aanpassen✏️" to="" />
                        <Knop label="Verwijderen🗑️" to="" />
                    </DashboardPanel>

                    <DashboardPanel
                        title="Orchidee 'Wit Elegance'"
                        imageSrc="/images/bloem.png"
                        veilingduur="2 min 30 sec"
                        totaalprijs="€ 22,40"
                        kloklocatie="Klok 4 - Hal D"
                    >
                        <Knop label="Aanpassen✏️" to="" />
                        <Knop label="Verwijderen🗑️" to="" />
                    </DashboardPanel>

                    <DashboardPanel
                        title="Gerbera Regenboog"
                        imageSrc="/images/bloem.png"
                        veilingduur="1 min 20 sec"
                        totaalprijs="€ 10,50"
                        kloklocatie="Klok 1 - Hal A"
                    >
                        <Knop label="Aanpassen✏️" to="" />
                        <Knop label="Verwijderen🗑️" to="" />
                    </DashboardPanel>

                    <DashboardPanel
                        title=""
                        imageSrc="/images/grey.png"
                        veilingduur=""
                        totaalprijs=""
                        kloklocatie=""
                    >
                        <Knop label="Veiling aanmaken" to="/toevoegen/product" />
                    </DashboardPanel>


                </div>

            </div>

        </>


    );
}
