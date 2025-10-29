"use client";

import s from '@/app/toevoegen/page.module.css'
import Submit from "@/components/input/submit";
import TextField from "@/components/input/textField";
import SelectField from "@/components/input/selectField";

export default function Home(){
    return (
        <form className={s.form}>
            <div className={s.multiRow}>
                <section className={s.section}>
                    <h1 className={s.h1}>Veiling Aanmaken</h1>
                    <TextField placeholder={"Starttijd"} />
                    <TextField placeholder={"Locatie"} />
                </section>
                <section className={s.section}>
                    <h3 className={s.h3}>Gekoppelde Producten</h3>
                    <SelectField
                        jsonData={{
                            Rose: { count: "12" },
                            Tulip: { count: "8" },
                            Daisy: { count: "15" },
                            Sunflower: { count: "6" },
                            Lily: { count: "9" },
                            Orchid: { count: "11" },
                            Marigold: { count: "7" },
                            Carnation: { count: "10" },
                            Lavender: { count: "5" },
                            Peony: { count: "13" },
                        }}
                    />
                </section>
            </div>
            <Submit placeholder={"Aanmaken"} />
        </form>
    );
}
