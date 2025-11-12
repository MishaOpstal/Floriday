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
            <main className={styles.main}>

                <div className={styles.page}>
                    <h1>Alle veilingen</h1>
                    <h2 className={styles.padding}>Huidige veilingen</h2>

                    <div className={styles.panels}>
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
                            kloklocatie="Klok 2 - Kantine"
                            resterendeTijd="10 nov 2025, 08:30"
                        >
                            <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
                        </DashboardPanel>
                    </div>



                    <h2 className={styles.padding}>Aankomende veilingen</h2>

                    <div className={styles.panels}>
                        <DashboardPanel
                            compact
                            imageSrc="/images/PIPIPOTATO.png"
                            kloklocatie="Klok 3 - Vergaderzaal B"
                            resterendeTijd="11 nov 2025, 12:00"
                        >
                            <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
                        </DashboardPanel>

                        <DashboardPanel
                            compact
                            imageSrc="/images/PIPIPOTATO.png"
                            kloklocatie="Klok 4 - Receptie"
                            resterendeTijd="12 nov 2025, 17:15"
                        >
                            <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
                        </DashboardPanel>
                    </div>

                    <h2 className={styles.padding}>Afgelopen veilingen</h2>

                    <div className={styles.panels}>
                        <DashboardPanel
                            compact
                            imageSrc="/images/PIPIPOTATO.png"
                            kloklocatie="Klok 5 - Werkplaats"
                            resterendeTijd="13 nov 2025, 09:45"
                        >
                            <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
                        </DashboardPanel>

                        <DashboardPanel
                            compact
                            imageSrc="/images/PIPIPOTATO.png"
                            kloklocatie="Klok 4 - Receptie"
                            resterendeTijd="12 nov 2025, 17:15"
                        >
                            <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
                        </DashboardPanel>

                    </div>

                </div>

            </main>

        </>


    );
}
