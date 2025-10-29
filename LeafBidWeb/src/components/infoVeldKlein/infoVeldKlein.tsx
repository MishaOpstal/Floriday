import s from './infoVeldKlein.module.css';

type InfoFieldProps = {
    label: string;
    value: string | number;
};

export default function InfoVeld({ label, value }: InfoFieldProps) {
    return (
        <div className={s.wrapper}>
            <span className={s.label}>{label}</span>
            <span className={s.value}>{value}</span>
        </div>
    );
}
