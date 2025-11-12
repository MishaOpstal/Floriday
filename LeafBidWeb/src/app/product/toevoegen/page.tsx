"use client";

// Import styling from the toevoegen layout
import s from '@/app/layouts/toevoegen/page.module.css'
import ToevoegenLayout from "@/app/layouts/toevoegen/layout"

import Form from "react-bootstrap/Form"
import TextInput from "@/components/input/TextInput";
import NumberInput from "@/components/input/NumberInput";
import FileInput from "@/components/input/FileInput";
import TextAreaInput from "@/components/input/TextAreaInput";
import Button from "@/components/input/Button";


export default function ProductForm() {
    return (
        <ToevoegenLayout>
            <Form className={s.form}>
                <h1>Product Toevoegen</h1>

                <TextInput label="Product Naam" name="productnaam" placeholder="naam" />
                <NumberInput label="Aantal" name="aantal" placeholder="aantal" step={1} />
                <FileInput label="afbeelding" name="afbeelding" />
                <NumberInput label="Minimale Prijs" name="minprijs" placeholder="min. prijs" step={0.01} />
                <TextAreaInput label="Product Informatie" name="productinformatie" placeholder="product informatie" />
                <Button variant="primary" type="button" label={"Aanmaken"} />
            </Form>
        </ToevoegenLayout>
    );
}
