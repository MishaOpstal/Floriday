import s from './infoVeldKlein.module.css';
import {parsePrice, Product} from "@/types/Product";
import {Image} from "react-bootstrap";

export default function InfoVeld({ product }: { product: Product }) {
    const imageSrc = `http://localhost:5001${product.picture}`;

    return (
        <div className={`d-flex align-items-center gap-3 ${s.textContainer}`}>
            <Image
                src={imageSrc}
                alt={product.name}
                className={`img-fluid ${s.image}`}
            />
            <div className="d-flex flex-column justify-content-center ps-3 gap-2">
                <h4 className="fw-bold">{product.name}</h4>
                <p className="text-muted">{parsePrice(product.minPrice ?? 0)}</p>
            </div>
        </div>
    );
}
