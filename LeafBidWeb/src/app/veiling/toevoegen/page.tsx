"use client";

import s from "@/app/layouts/toevoegen/page.module.css";
import ToevoegenLayout from "@/app/layouts/toevoegen/layout";

import Form from "react-bootstrap/Form";
import OrderedMultiSelect from "@/components/input/OrderedMultiSelect";
import { Product } from "@/types/Product";
import { Locatie } from "@/types/Locatie";
import React, { useState } from "react";
import DateSelect from "@/components/input/DateSelect";
import SearchableDropdown from "@/components/input/SearchableDropdown";
import Button from "@/components/input/Button";
import ProductPriceTable from "@/components/input/ProductPriceTable";

// Dummy data
const products: Product[] = [
    { id: 1, name: "Rose", stock: 12 },
    { id: 2, name: "Tulip", stock: 8 },
    { id: 3, name: "Daisy", stock: 15 },
    { id: 4, name: "Sunflower", stock: 6 },
    { id: 5, name: "Lily", stock: 9 },
    { id: 6, name: "Orchid", stock: 11 },
    { id: 7, name: "Marigold", stock: 7 },
    { id: 8, name: "Carnation", stock: 10 },
    { id: 9, name: "Lavender", stock: 5 },
    { id: 10, name: "Peony", stock: 13 },
    { id: 11, name: "Chillsy", stock: 13 },
    { id: 12, name: "Deku", stock: 13 },
    { id: 13, name: "Misha", stock: 13 },
    { id: 14, name: "Pootsvis", stock: 13 },
    { id: 15, name: "Sweetie", stock: 13 },
    { id: 16, name: "Tulpenmix", stock: 13 },
    { id: 17, name: "Zomergeur", stock: 13 },
    { id: 18, name: "Zomergeur", stock: 13 },
    { id: 19, name: "Zomergeur", stock: 13 },
];

const locaties: Locatie[] = [
    { locatieId: 1, locatieNaam: "Aalsmeer" },
    { locatieId: 2, locatieNaam: "Rijnsburg" },
    { locatieId: 3, locatieNaam: "Naaldwijk" },
    { locatieId: 4, locatieNaam: "Eelde" },
];

export default function Home() {
    const [selectedDate, setSelectedDate] = useState<string | null>(null);
    const [selectedLocatie, setSelectedLocatie] = useState<string | null>(null);
    const [selectedProducts, setSelectedProducts] = useState<Product[]>([]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        console.log("Form submitted:");
        console.log({
            starttijd: selectedDate,
            locatie: selectedLocatie,
            producten: selectedProducts,
        });
    };

    const handlePriceUpdate = (updated: Product[]) => {
        setSelectedProducts(updated);
    };

    return (
        <ToevoegenLayout>
            <Form className={s.form} onSubmit={handleSubmit}>
                <div className={s.multiRow}>
                    <section className={s.section}>
                        <h1 className={s.h1}>Veiling Aanmaken</h1>

                        {/* Date picker */}
                            <Form.Label>Startdatum en tijd</Form.Label>
                            <DateSelect
                                placeholder="Selecteer startdatum"
                                onSelect={setSelectedDate}
                                useTime={true} label={""}                            />

                        {/* Location dropdown */}
                            <Form.Label>Locatie
                            <SearchableDropdown
                                label="Selecteer locatie"
                                items={locaties}
                                displayKey="locatieNaam"
                                valueKey="locatieId"
                                onSelect={(loc) => setSelectedLocatie(loc.locatieNaam)}
                                placeholder="Zoek locatie..."
                            /></Form.Label>

                        <ProductPriceTable
                            products={selectedProducts}
                            onChange={handlePriceUpdate}
                            height={300}
                        />
                    </section>

                    <section className={s.section}>
                        <h3 className={s.h3}>Gekoppelde Producten</h3>
                        <OrderedMultiSelect
                            items={products}
                            value={selectedProducts} // keeps it in sync with parent
                            onChange={setSelectedProducts}
                            showBadges={false}
                        />
                    </section>
                </div>

                <Button
                    variant="primary"
                    type="button"
                    label="Aanmaken"
                    onClick={handleSubmit}
                />
            </Form>
        </ToevoegenLayout>
    );
}
