'use client';
import styles from './page.module.css';
import Header from "@/components/header/header";
import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";
import { useState, useEffect } from "react";

// Types
type Auction = {
    id: number;
    startDate: string;
    clockLocationEnum: number;
    auctioneerId: number;
    products: Product[]; // product(s) zitten hier in
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

export default function Home() {
    const [auctions, setAuctions] = useState<Auction[]>([]);
    const [loading, setLoading] = useState(true);

    const id = 7;

    useEffect(() => {
        const fetchData = async () => {
            try {
                const res = await fetch(`http://localhost:5001/api/v1/Pages/${id}`);
                if (!res.ok) throw new Error("Failed to fetch auction+product");
                const data = await res.json();


                const auctionWithProducts: Auction = {
                    ...data.auction,
                    products: [data.product],
                };

                setAuctions([auctionWithProducts]);
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [id]);

    return (
        <>
            <Header />
            <main className={styles.main}>
                <div className={styles.page}>
                    <h1 className={styles.huidigeVeilingen}>Veilingen Dashboard</h1>
                    <div className={styles.panels}>
                        {loading ? (
                            <DashboardPanel loading={true} title="Laden..." />
                        ) : auctions.length === 0 ? (
                            <DashboardPanel loading={true} title="Geen veilingen beschikbaar" />
                        ) : (
                            auctions.map((auction) => {
                                const product = auction.products[0];

                                return (
                                    <DashboardPanel
                                        key={auction.id}
                                        loading={false}
                                        title={product ? product.name : `Auction #${auction.id}`}
                                        kloklocatie={`Clock: ${auction.clockLocationEnum}`}
                                        imageSrc={"/images/PIPIPOTATO.png"}
                                        resterendeTijd={new Date(auction.startDate).toLocaleString()}
                                        huidigePrijs={product?.minPrice || "—"}
                                        aankomendProductNaam={product?.name || "Geen product"}
                                        aankomendProductStartprijs={product?.maxPrice || "—"}
                                    />
                                );
                            })
                        )}
                    </div>
                </div>
            </main>
        </>
    );
}
