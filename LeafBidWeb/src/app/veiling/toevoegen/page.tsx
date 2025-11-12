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
    { productId: 1, productName: "Rose", productQuantity: 12 },
    { productId: 2, productName: "Tulip", productQuantity: 8 },
    { productId: 3, productName: "Daisy", productQuantity: 15 },
    { productId: 4, productName: "Sunflower", productQuantity: 6 },
    { productId: 5, productName: "Lily", productQuantity: 9 },
    { productId: 6, productName: "Orchid", productQuantity: 11 },
    { productId: 7, productName: "Marigold", productQuantity: 7 },
    { productId: 8, productName: "Carnation", productQuantity: 10 },
    { productId: 9, productName: "Lavender", productQuantity: 5 },
    { productId: 10, productName: "Peony", productQuantity: 13 },
    { productId: 11, productName: "Chillsy", productQuantity: 13 },
    { productId: 12, productName: "Deku", productQuantity: 13 },
    { productId: 13, productName: "Misha", productQuantity: 13 },
    { productId: 14, productName: "Pootsvis", productQuantity: 13 },
    { productId: 15, productName: "Sweetie", productQuantity: 13 },
    { productId: 16, productName: "Tulpenmix", productQuantity: 13 },
    { productId: 17, productName: "Zomergeur", productQuantity: 13 },
    { productId: 18, productName: "Zomergeur", productQuantity: 13 },
    { productId: 19, productName: "Zomergeur", productQuantity: 13 },
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
        console.log("Updated prices:", updated);
    };

    return (
        <ToevoegenLayout>
            <Form className={s.form} onSubmit={handleSubmit}>
                <div className={s.multiRow}>
                    <section className={s.section}>
                        <h1 className={s.h1}>Veiling Aanmaken</h1>

                        {/* Date picker */}
                        <div className="mb-3">
                            <Form.Label>Startdatum en tijd</Form.Label>
                            <DateSelect
                                placeholder="Selecteer startdatum"
                                onSelect={setSelectedDate}
                                useTime={true}
                            />
                        </div>

                        {/* Location dropdown */}
                        <div className="mb-3">
                            <Form.Label>Locatie</Form.Label>
                            <SearchableDropdown
                                label="Selecteer locatie"
                                items={locaties}
                                displayKey="locatieNaam"
                                valueKey="locatieId"
                                onSelect={(loc) => setSelectedLocatie(loc.locatieNaam)}
                                placeholder="Zoek locatie..."
                            />
                        </div>

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
