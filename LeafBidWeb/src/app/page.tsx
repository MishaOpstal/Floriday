'use client';
import styles from './page.module.css';
import Header from "@/components/header/header";
import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";
import { useState, useEffect } from "react";
import {Auction} from "@/types/Auction";


export default function Home() {
    const [auctions, setAuctions] = useState<Auction[]>([]);
    const [loading, setLoading] = useState(true);

    const id = 1;

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
                                        imageSrc={"http://localhost:5001" + product?.picture}
                                        resterendeTijd={new Date(auction.startDate).toLocaleString()}
                                        huidigePrijs={product?.minPrice}
                                        aankomendProductNaam={product?.name || "Geen product"}
                                        aankomendProductStartprijs={product?.maxPrice}
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
