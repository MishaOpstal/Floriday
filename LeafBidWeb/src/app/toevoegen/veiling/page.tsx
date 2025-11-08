"use client";

import s from '@/app/toevoegen/page.module.css'
import SelectField from "@/components/input/selectField";
import Form from "react-bootstrap/Form"
import Button from "react-bootstrap/Button";

export default function Home(){
    return (
        <Form className={s.form}>
            <div className={s.multiRow}>
                <section className={s.section}>
                    <h1 className={s.h1}>Veiling Aanmaken</h1>
                    <Form.Control type="date" placeholder="Starttijd" name="starttijd"/>
                    <Form.Control type="dropdown" placeholder="Locatie" name="locatie"/>
                </section>
                <section className={s.section}>
                    <h3 className={s.h3}>Gekoppelde Producten</h3>
                    {/*<SelectField*/}
                    {/*    jsonData={{*/}
                    {/*        Rose: { count: "12" },*/}
                    {/*        Tulip: { count: "8" },*/}
                    {/*        Daisy: { count: "15" },*/}
                    {/*        Sunflower: { count: "6" },*/}
                    {/*        Lily: { count: "9" },*/}
                    {/*        Orchid: { count: "11" },*/}
                    {/*        Marigold: { count: "7" },*/}
                    {/*        Carnation: { count: "10" },*/}
                    {/*        Lavender: { count: "5" },*/}
                    {/*        Peony: { count: "13" },*/}
                    {/*    }}*/}
                    {/*/>*/}
                </section>
            </div>
            <Form.Control as={Button} type="submit"  value="Aanmaken">Aanmaken</Form.Control>
        </Form>
    );
}
