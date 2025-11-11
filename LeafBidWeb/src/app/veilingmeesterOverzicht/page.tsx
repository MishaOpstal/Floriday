'use client';

import styles from '../page.module.css';
import Header from "@/components/header/header";
import ActionButtons from "@/components/smallButton/smallButton";
import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";

export default function Home() {

    const handleDelete = () => {
        // TODO : implement delete functionality
    };

    const handleUpdate = () => {
        // TODO : implement update functionality
    };


    return (
        <>
            <Header></Header>
            <div className={styles.page}>

                <div className={styles.main}>
                    <h1>Alle veilingen</h1>
                    <h2>Huidige veilingen</h2>

                    <DashboardPanel
                        compact
                        imageSrc="/images/PIPIPOTATO.png"
                        kloklocatie="Klok 1 - Hal A"
                        resterendeTijd="9 nov 2025, 16:45"
                    >
                        <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
                    </DashboardPanel>



                    <DashboardPanel
                        compact
                        imageSrc="/images/PIPIPOTATO.png"
                        kloklocatie="Klok 2 - Hal B"
                        resterendeTijd="9 nov 2025, 17:00" />

                    <h2>Aankomende veilingen</h2>
                    <DashboardPanel
                        compact
                        imageSrc="/images/PIPIPOTATO.png"
                        kloklocatie="Klok 3 - Hal C"
                        resterendeTijd="9 nov 2025, 17:15" />

                    <DashboardPanel
                        compact
                        imageSrc="/images/PIPIPOTATO.png"
                        kloklocatie="Klok 4 - Hal D"
                        resterendeTijd="9 nov 2025, 17:30" />
                    <h2>Afgelopen veilingen</h2>
                    <DashboardPanel
                        compact
                        imageSrc="/images/PIPIPOTATO.png"
                        kloklocatie="Klok 5 - Hal E"
                        resterendeTijd="9 nov 2025, 17:45" />

                    <DashboardPanel
                        compact
                        imageSrc="/images/PIPIPOTATO.png"
                        kloklocatie="Klok 5 - Hal E"
                        resterendeTijd="9 nov 2025, 17:45" />

                </div>

            </div>

        </>


    );
}
