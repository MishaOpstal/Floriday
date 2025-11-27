'use client';
import styles from './page.module.css';
import Header from "@/components/header/header";
import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";
import {useState, useEffect} from "react";
import {Auction} from "@/types/Auction";
import {parseClockLocation} from "@/enums/ClockLocation";
import AuctionTimer from "@/components/veilingKlok/veilingKlok";


const auctionIdList = [1, 2, 3, 4];

export default function Home() {
    const [auctions, setAuctions] = useState<Auction[]>([]);
    const [loading, setLoading] = useState(true);


    useEffect(() => {
        const fetchAuctions = async () => {
            setLoading(true);

            try {
                // Fetch all auctions in parallel
                const results = await Promise.all(
                    auctionIdList.map(async (id) => {
                        const res = await fetch(`http://localhost:5001/api/v1/Pages/${id}`);
                        if (!res.ok) return null;

                        const data = await res.json();

                        // Normalize product+auction into your Auction type
                        const auction: Auction = {
                            ...data.auction,
                            products: data.products
                        };

                        return auction;
                    })
                );

                // Filter out null responses (failed fetches)
                const filtered = results.filter((a): a is Auction => a !== null);

                setAuctions(filtered);
            } catch (err) {
                console.error("Failed to load auctions:", err);
            } finally {
                setLoading(false);
            }
        };

        fetchAuctions();
    }, []);


    return (
        <>
            <Header/>
            <main className={styles.main}>
                <div className={styles.page}>
                    <h1 className={styles.huidigeVeilingen}>Veilingen Dashboard</h1>

                    <div className={styles.panels}>
                        {loading ? (
                            <>
                                <DashboardPanel loading={true} title="Laden..."/>
                                <DashboardPanel loading={true} title="Laden..."/>
                                <DashboardPanel loading={true} title="Laden..."/>
                                <DashboardPanel loading={true} title="Laden..."/></>
                        ) : auctions.length === 0 ? (
                            <DashboardPanel loading={true} title="Geen veilingen beschikbaar"/>
                        ) : (
                            auctions.map((auction) => {
                                const product = auction.products?.[0];
                                const nextProduct = auction.products?.[1];



                                return (
                                    <>
                                        <a key={`auction-${auction.id}`} href={`/veiling/${auction.id}`}>
                                            <DashboardPanel
                                                key={auction.id}
                                                loading={false}
                                                title={product ? product.name : `Auction #${auction.id}`}
                                                kloklocatie={parseClockLocation(auction.clockLocationEnum)}
                                                imageSrc={product?.picture ? `http://localhost:5001/uploads/${product.picture}` : undefined}
                                                resterendeTijd={new Date(auction.startDate).toLocaleString()}
                                                huidigePrijs={product?.minPrice}
                                                aankomendProductNaam={nextProduct?.name || "Geen product"}
                                                aankomendProductStartprijs={nextProduct?.minPrice}
                                            />
                                        </a>
                                    </>
                                );
                            })
                        )}
                    </div>

                </div>
            </main>
        </>
    );
}
