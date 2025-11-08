"use client";

import s from '@/app/toevoegen/page.module.css'
import Form from "react-bootstrap/Form"
import Button from "react-bootstrap/Button"
import {useSearchParams} from "next/navigation";

interface ProductProps {
    type: "add" | "edit";
    productId?: number;
}


export default function ProductForm() {
    const searchParams = useSearchParams();

    // Extract values from URL
    const typeParam = searchParams.get("type");
    const productIdParam = searchParams.get("productId");

    // Convert them to your typed props
    const props: ProductProps = {
        type: typeParam === "edit" ? "edit" : "add",
        productId: productIdParam ? Number(productIdParam) : undefined,
    };
    return (
            <Form className={s.form}>
                <h1>Product {props.type === "add" ? "Aanmaken" : "Bewerken"}</h1>

                <Form.Label>Product Naam
                    <Form.Control type="text" placeholder="naam" name="productnaam" />
                </Form.Label>

                <Form.Label>Aantal
                    <Form.Control type="number" step="1" placeholder="aantal" name="aantal" />
                </Form.Label>

                <Form.Label>Plaatje
                    <Form.Control type="file" placeholder="plaatje" name="plaatje" />
                </Form.Label>

                <Form.Label>Minimale Prijs
                    <Form.Control type="number" step="0.01" placeholder="min. prijs" name="minprijs" />
                </Form.Label>

                <Form.Label>Product Informatie
                    <Form.Control as="textarea" rows={3} placeholder="product informatie" name="productinformatie" />
                </Form.Label>

                <Form.Control as={Button} type="submit"  value="Aanmaken">Aanmaken</Form.Control>
            </Form>
    );
}
