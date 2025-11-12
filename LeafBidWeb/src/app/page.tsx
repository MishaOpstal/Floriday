'use client';

import styles from './page.module.css';
import Header from "@/components/header/header";
import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";
import { useState, useEffect } from "react";

// Define User type
type User = {
    id: number;
    name: string;
    email: string;
    passwordHash: string;
    userType: number;
};

export default function Home() {
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const res = await fetch("http://localhost:5001/api/v1/User"); // adjust port if needed
                if (!res.ok) throw new Error("Failed to fetch users");
                const data = await res.json();
                setUsers(data);
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchUsers();
    }, []);

    return (
        <>
            <Header />
            <main className={styles.main}>
                <div className={styles.page}>
                    <h1 className={styles.huidigeVeilingen}>Gebruikers Dashboard</h1>
                    <div className={styles.panels}>
                        {loading ? (
                            // CASE 1: Loading
                            <DashboardPanel loading={true} title="Laden..." />
                        ) : users.length === 0 ? (
                            // CASE 2: No data
                            <DashboardPanel loading={true} title="Geen gebruikers beschikbaar" />
                        ) : (
                            // CASE 3: Show user data
                            users.map((user) => (
                                <DashboardPanel
                                    key={user.id}
                                    loading={false}
                                    title={`Gebruiker: ${user.name}`}
                                    kloklocatie={`User ID: ${user.id}`}
                                    imageSrc="/images/PIPIPOTATO.png"
                                    resterendeTijd="—"
                                    huidigePrijs={`Type: ${user.userType}`}
                                    aankomendProductNaam={user.email}
                                    aankomendProductStartprijs={user.passwordHash}
                                />
                            ))
                        )}
                    </div>
                </div>
            </main>
        </>
    );
}


