import s from '@/app/toevoegen/page.module.css'
import NumberField from "@/components/input/numberField";
import TextArea from "@/components/input/textArea";
import Submit from "@/components/input/submit";

export default function Home() {
    return (
            <form className={s.form}>
                <h1 className={s.h1}>Product Toevoegen</h1>
            <div className={s.multiRow}>
                <NumberField placeholder={"Min Prijs"} step={0.01} />
                <NumberField placeholder={"Max Prijs"} step={0.01} />
            </div>
            <NumberField placeholder={"Aantal"} step={1} />
            <TextArea placeholder={"Product Informatie"} />
            <Submit placeholder={"Aanmaken"} />
            </form>
    );
}
