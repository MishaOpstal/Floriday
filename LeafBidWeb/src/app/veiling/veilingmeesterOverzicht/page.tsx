'use client';

import styles from '../../page.module.css';
import Header from "@/components/header/header";
import ActionButtons from "@/components/smallButton/smallButton";
import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";
import { useState, useEffect } from "react";

// Types
type Auction = {
    id: number;
    startDate: string;
    clockLocationEnum: number;
    auctioneerId: number;
    products: Product[];
};

type Product = {
    id: number;
    name: string;
    description: string;
    minPrice: string;
    maxPrice: string;
    weight: number;
    picture: string;
    species: string;
    region: string;
    potSize: number;
    stemLength: number;
    stock: number;
    harvestedAt: string;
    providerId: number;
    auctionId: number;
};

type PageResponse = {
    auction: Auction;
    product: Product;
};

export default function Home() {
    const [auctions, setAuctions] = useState<Auction[]>([]);

    const handleDelete = () => {
        // TODO : implement delete functionality
    };

    const handleUpdate = () => {
        // TODO : implement update functionality
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const res = await fetch("http://localhost:5001/api/v1/Pages/7");
                if (!res.ok) throw new Error("Failed to fetch auction+product");

                const data: PageResponse = await res.json();

                // Combineer auction + product in één object
                const auctionWithProducts: Auction = {
                    ...data.auction,
                    products: [data.product],
                };

                setAuctions([auctionWithProducts]);
            } catch (err) {
                console.error(err);
            }
        };

        fetchData();
    }, []);

    return (
        <>
            <Header />
            <main className={styles.main}>
                <div className={styles.page}>
                    <h1>Alle veilingen</h1>
                    <h2 className={styles.padding}>Huidige veilingen</h2>

                    <div className={styles.panels}>

                    </div>



                    <h2 className={styles.padding}>Aankomende veilingen</h2>

                    <div className={styles.panels}>

                    </div>

                    <h2 className={styles.padding}>Afgelopen veilingen</h2>

                    <div className={styles.panels}>


                    </div>
                    {auctions.map((auction) => {
                        const product = auction.products[0];
                        return (
                            <DashboardPanel
                                key={auction.id}
                                compact
                                imageSrc={"/images/PIPIPOTATO.png"}
                                kloklocatie={`Klok ${auction.clockLocationEnum}`}
                                resterendeTijd={new Date(auction.startDate).toLocaleString()}
                            >
                                <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
                            </DashboardPanel>
                        );
                    })}
                </div>
            </main>
        </>
    );
}



// 'use client';
//
// import styles from '../page.module.css';
// import Header from "@/components/header/header";
// import ActionButtons from "@/components/smallButton/smallButton";
// import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";
//
// export default function Home() {
//
//     const handleDelete = () => {
//         // TODO : implement delete functionality
//     };
//
//     const handleUpdate = () => {
//         // TODO : implement update functionality
//     };
//
//
//     return (
//         <>
//             <Header></Header>
//             <main className={styles.main}>
//
//                 <div className={styles.page}>
//                     <h1>Alle veilingen</h1>
//                     <h2>Huidige veilingen</h2>
//
//                     <DashboardPanel
//                         compact
//                         imageSrc="/images/PIPIPOTATO.png"
//                         kloklocatie="Klok 1 - Hal A"
//                         resterendeTijd="9 nov 2025, 16:45"
//                     >
//                         <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
//                     </DashboardPanel>
//
//
//
//                     <DashboardPanel
//                         compact
//                         imageSrc="/images/PIPIPOTATO.png"
//                         kloklocatie="Klok 2 - Kantine"
//                         resterendeTijd="10 nov 2025, 08:30"
//                     >
//                         <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
//                     </DashboardPanel>
//
//                     <h2>Aankomende veilingen</h2>
//                     <DashboardPanel
//                         compact
//                         imageSrc="/images/PIPIPOTATO.png"
//                         kloklocatie="Klok 3 - Vergaderzaal B"
//                         resterendeTijd="11 nov 2025, 12:00"
//                     >
//                         <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
//                     </DashboardPanel>
//
//                     <DashboardPanel
//                         compact
//                         imageSrc="/images/PIPIPOTATO.png"
//                         kloklocatie="Klok 4 - Receptie"
//                         resterendeTijd="12 nov 2025, 17:15"
//                     >
//                         <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
//                     </DashboardPanel>
//                     <h2>Afgelopen veilingen</h2>
//                     <DashboardPanel
//                         compact
//                         imageSrc="/images/PIPIPOTATO.png"
//                         kloklocatie="Klok 5 - Werkplaats"
//                         resterendeTijd="13 nov 2025, 09:45"
//                     >
//                         <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
//                     </DashboardPanel>
//
//                     <DashboardPanel
//                         compact
//                         imageSrc="/images/PIPIPOTATO.png"
//                         kloklocatie="Klok 4 - Receptie"
//                         resterendeTijd="12 nov 2025, 17:15"
//                     >
//                         <ActionButtons onDelete={handleDelete} onUpdate={handleUpdate} />
//                     </DashboardPanel>
//
//                 </div>
//
//             </main>
//
//         </>
//
//
//     );
// }
